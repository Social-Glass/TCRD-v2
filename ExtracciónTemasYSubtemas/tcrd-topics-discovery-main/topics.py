import json
import os
import re
import PyPDF2
import pandas as pd

def parse_pdf(): 
    # read all pdf files from data folder
    pdfs = []
    for root, dirs, files in os.walk("data/bulletins"):
        for file in files:
            if file.endswith(".pdf"):
                pdfs.append(os.path.join(root, file))

    for file_name in pdfs:
        print(f"Reading {file_name}")
        file_name_parts = file_name.split("/")
        bulletin_file_name = file_name_parts[-1].split(".")[0]
        print(f"Writing {bulletin_file_name}")
        bulletin_year = re.search(r"(\d{4})", file_name).group(1)
        print(f"Year {bulletin_year}")
        topics = get_topics(file_name)
        json.dump(topics, open(f"data/bulletins/bulletin-{bulletin_year}-{bulletin_file_name}.json", "w"), indent=4)
        print(f"Done {bulletin_file_name}\n")

# get topic index page number
def get_topic_index_page(reader):
    for num in reversed(range(len(reader.pages))):
        page = reader.pages[num]
        text = page.extract_text()
        found = re.search(r"INDICE TEMÁTICO|ÍNDICE TEMÁTICO", text)
        if found:
            return num

# get sentences from line        
def get_sentences_from_line(text):
    sentences = []
    for sentence_number in text.split(";"):
        if (not sentence_number.strip().isdigit()):
            continue
        sentences.append(int(sentence_number.strip()))
    return sentences    

# get topics from pdf file
def get_topics(file_name):
    file_name_parts = file_name.split("/")
    bulletin_file_path = "/".join(file_name_parts[-2:])
    bulletin_file_name = file_name_parts[-1].split(".")[0]
    bulletin_year = re.search(r"(\d{4})", file_name).group(1)
    reader = PyPDF2.PdfReader(file_name)
    topic_index_page_number = get_topic_index_page(reader)
    topics = []
    last_parent_topic = ""
    for num in (range(topic_index_page_number, len(reader.pages))):
        page = reader.pages[num]
        page_text = page.extract_text()
        lines = page_text.splitlines()
        last_line = ""
        for line in lines:
            # last parent topic equals when all characters in line are uppercase
            if line.strip().isupper():
                last_parent_topic = line
                # regex to remove all digits and commas from last parent topic
                last_parent_topic = re.sub(r"\d+|,|;", "", last_parent_topic)
            parts = line.rsplit(",", 1)
            if (len(parts) >= 2):
                topic_name = " ".join([last_line.strip(), parts[0]])
                last_line = ""
                sentences = get_sentences_from_line(parts[1])
                topics.append({ 
                    "file": bulletin_file_path, 
                    "file_name": bulletin_file_name, 
                    "year": bulletin_year, 
                    'parent': last_parent_topic.strip(),
                    "name": topic_name.strip(), 
                    "sentences": sentences, 
                })
            else:
                if re.search(r"\d+;", line):
                    # get last topic from topics and append sentences
                    if len(topics) > 0:
                        topics[-1]["sentences"].extend(get_sentences_from_line(line))
                    continue
                last_line = " - ".join([last_line, line]) if last_line != "" else line.strip()
    return topics

if __name__ == "__main__":
    parse_pdf()