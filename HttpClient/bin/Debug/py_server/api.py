import json
import requests
import base64
import cv2
def test_api(url,data):
    r = requests.post(url, data)
    print(r.json())
    # return r.json()

def image_to_base64(image_np):
    image = cv2.imencode('.jpg', image_np)[1]
    image_code = str(base64.b64encode(image))[2:-1]
    return image_code
pic = cv2.imread('./image/002.jpg')
pic = cv2.resize(pic, (1365, 1000))
b64_pic = image_to_base64(pic)
req_data = {"imageCount": "ff","b64Image": b64_pic}
json_req = json.dumps(req_data)

if __name__ == '__main__':
    base_url = 'http://127.0.0.1:8087/particle/'
    req_heatmap_url = base_url + 'heatmap'
    test_api(req_heatmap_url, json_req)