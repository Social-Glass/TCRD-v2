# Downloads sentences PDF files from the TC portal
# Do a wide search for all sentences
# Save the downloaded sentence pdf to disk
import os
import re
from bs4 import BeautifulSoup
import requests
import json

base_url = "https://tcstgsite.azurewebsites.net/consultas/secretar%C3%ADa/sentencias/?SearchGeneral=&SearchAllWords=&SearchExactPhrase=&SearchAnyWord=&SearchNoneOfWord=&SearchDecide=&anos=&procesos=&tema=&magistrado=&magistradoDisidente="

def get_page(url):
    response = requests.get(url)
    if not response.ok:
        print("Server responded: ", response.status_code)
    else:
        soup = BeautifulSoup(response.text, "html.parser")
    return soup

def get_sentences_links(soup):
    links = soup.find_all("a", href=lambda href: href and "/consultas/secretaría/sentencias/tc" in href)
    urls = [link.get("href") for link in links]
    return urls

def get_sentence_pdf_link(soup):
    link = soup.find("a", string=re.compile("Mostrar PDF en otra pestaña"))
    url = link.get("href")
    return url

def download_pdf(url):
    print("Downloading ", url)
    file_name = url.split("/")[-1]
    response = requests.get(url)
    if not response.ok:
        print("Server responded: ", response.status_code)
    else:
        os.makedirs("data/sentences", exist_ok=True)
        with open(f"data/sentences/{file_name}", "wb") as file:
            file.write(response.content)
            print(f"{file_name} downloaded")

def main(url):
    # get urls from file, else fetch urls from base url
    if os.path.exists("data/sentences_urls.json"):
        with open("data/sentences_urls.json") as file:
            urls = json.load(file)
    else:
        soup = get_page(url)
        urls = get_sentences_links(soup)

    print(f"Found {len(urls)} urls")
    
    # save urls to file
    with open("data/sentences_urls.json", "w") as file:
        json.dump(urls, file, indent=4)

    for sentence_url in urls:
        sentence_url = f"https://tcstgsite.azurewebsites.net{sentence_url}"
        print(f"working with {sentence_url}")
        sentence_soup = get_page(sentence_url)
        sentence_pdf_link = get_sentence_pdf_link(sentence_soup)
        download_pdf(sentence_pdf_link)

if __name__ == "__main__":
    main(base_url)