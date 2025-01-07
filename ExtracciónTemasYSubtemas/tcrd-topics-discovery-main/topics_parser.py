# convert json to csv
import io
import pandas as pd
from pandas import json_normalize
import sys
import json

"""
Converts json file to csv file
take json file as input from argument
"""
def convert_json_to_csv(json_file):
    # read json file
    with open(json_file, 'r') as f:
        data = json.load(f)
    # normalize json data
    df = json_normalize(data)
    df = df.explode('sentences')
    # write to csv file
    df.to_csv(json_file.replace('.json', '.csv'), index=False)

if __name__ == "__main__":
    # get file name from argument
    file_name = sys.argv[1]
    convert_json_to_csv(file_name)