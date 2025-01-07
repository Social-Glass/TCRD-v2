# read pdf from data/sentences and extract text from it after that look for "II. CONSIDERACIONES Y FUNDAMENTOS" and save the starting position, look for "DECIDE:" and save the index of the result. Then save to a variable the text between the two indexes and save it to a file in data/extracted_text
import os
import PyPDF2

def get_text_from_pdf(pdf_file):
    # read pdf file
    pdf_file = open(pdf_file, 'rb')
    reader = PyPDF2.PdfReader(pdf_file)
    # extract text from pdf
    text = ""
    for page_number in range(len(reader.pages)):
        page = reader.pages[page_number]
        text += page.extract_text()
    # close pdf file
    pdf_file.close()
    return text

def get_text_between_indexes(text, start_index, end_index):
    return text[start_index:end_index]

def get_indexes_from_text(text, start_text, end_text):
    start_index = text.find(start_text)
    end_index = text.find(end_text)
    return start_index, end_index

def save_text_to_file(text, file_name):
    # create new file and create directory if not exists
    os.makedirs(os.path.dirname(file_name), exist_ok=True)
    # open file
    file = open(file_name, "w")
    # write text to file
    file.write(text)
    # close file
    file.close()

def extract_text(file_name):
    # get text from pdf file
    text = get_text_from_pdf(file_name)
    # get indexes from text
    start_index, end_index = get_indexes_from_text(text, "II. CONSIDERACIONES Y FUNDAMENTOS", "DECIDE:")
    # get text between indexes
    text = get_text_between_indexes(text, start_index, end_index)
    # save text to file
    save_text_to_file(text, f"data/extracted_sentences_text/{file_name.split('/')[-1].split('.')[0]}.txt")

def extract_text_from_all_sentences():
    pdfs = []
    for root, dirs, files in os.walk("data/sentences"):
        for file in files:
            if file.endswith(".pdf"):
                pdfs.append(os.path.join(root, file))
    for file_name in pdfs:
        print(f"Extracting text from {file_name}")
        extract_text(file_name)
        print(f"Done {file_name}\n")

if __name__ == "__main__":
    extract_text_from_all_sentences()