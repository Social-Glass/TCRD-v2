# read files from data/extracted_sentences_text directory, take the sentence_idd from the file name (sentence_id = TC-####-##) and the text from the file content and make a post request to the api to create a new sentence

import os
import re
import requests
import json
import logging
from requests.adapters import HTTPAdapter, Retry

request_session = requests.Session()
basic = requests.auth.HTTPBasicAuth("c-a.delrosario@tc.gob.do", "")
request_session.auth = basic
request_session.headers.update({"Content-Type": "application/json"})

retries = Retry(
    total=5,
    allowed_methods=["POST"],
    backoff_factor=2,
    status_forcelist=[429, 500, 502, 503, 504],
)

request_session.mount("https://", adapter=HTTPAdapter(max_retries=retries))

logging.basicConfig(level=logging.DEBUG)


def get_text_from_file(file_name):
    # open file
    file = open(file_name, "r")
    # read text
    text = file.read()
    # close file
    file.close()
    return text


def get_sentence_id_from_file_name(file_name):
    print(f"getting sentence_id from file_name: {file_name}")
    file_name = file_name.lower().replace("_", "-")
    re_result = re.search(r"tc-\d+-\d{2}", file_name)
    if not re_result:
        print("sentence_id not found")
        return
    sentence_id = re_result.group()
    return sentence_id


def send_post_request(sentence_id, text):
    sentence_id = sentence_id.upper().replace("-", "/").strip()

    if not sentence_id:
        print("sentence_id is empty\n")
        return False

    print(f"\nsending post request for sentence_id: {sentence_id}")

    if os.path.isfile("data/updated_sentences.bak"):
        file = open("data/updated_sentences.bak", "r")
        updated_sentences = file.read()
        file.close()
        if sentence_id in updated_sentences:
            return False
        else:
            print("not in updated_sentences.bak")

    url = "https://tcstgsite.azurewebsites.net/umbraco/api/sentencia/setfundaments"
    payload = {"file_name": sentence_id, "fundaments": text}

    # do request with retry and exponential backoff
    try:
        response = request_session.post(url, data=json.dumps(payload))

        if response.ok:
            file = open("data/updated_sentences.bak", "a")
            file.write(sentence_id + "\n")
            file.close()
            return True
        else:
            print("error while sending post request")
            print(response.text)
            return False

    except Exception as e:
        # save sentence_id to skipped_sentences.txt
        file = open("data/skipped_sentences.txt", "a")
        file.write(sentence_id + "\n")
        file.close()
        return False


def main():
    # read all files from data/extracted_sentences_text directory
    txts = []
    for root, dirs, files in os.walk("data/extracted_sentences_text"):
        for file in files:
            if file.endswith(".txt"):
                txts.append(os.path.join(root, file))

    # for each file
    for file_name in txts:
        # get sentence_id from file name
        sentence_id = get_sentence_id_from_file_name(file_name)
        if not sentence_id:
            continue
        # get text from file
        text = get_text_from_file(file_name)
        # send post request
        request_sent = send_post_request(sentence_id, text)
        if not request_sent:
            print(f"skiped sentence_id: {sentence_id}")


if __name__ == "__main__":
    main()
