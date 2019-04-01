#!/usr/bin/env python
# encoding=utf-8
# coding=utf-8
from __future__ import absolute_import
from __future__ import division
from __future__ import print_function
import numpy as np
from inference import Particle
import logging,os,sys
import flask
import json
import traceback
import time
from PIL import Image
from io import BytesIO
import base64
import cv2
from random import randint

def image_to_base64(image_np):
    image = cv2.imencode('.jpg', image_np)[1]
    image_code = str(base64.b64encode(image))[2:-1]
    return image_code

def base64_to_image(b64):
    jpg_data = BytesIO(base64.b64decode(b64))
    pil_image = Image.open(jpg_data)
    return cv2.cvtColor(np.asarray(pil_image), cv2.COLOR_RGB2BGR)

app = flask.Flask(__name__)
@app.route('/particle/heatmap', methods=['POST'])
def get_heatmap():
    start_time = time.time()
    try:
        para = json.loads(flask.request.get_data())
        siteid = para['imageCount']
        request_picture = para['b64Image']
        image = base64_to_image(request_picture)
        # (2048, 1500)
        heatmap = _particle.detect_whole(image, (image.shape[0] // 2,image.shape[1] // 3))
        _, thresh = cv2.threshold(heatmap, 127, 255, cv2.THRESH_BINARY)
        _, contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
        # show the contours of the imput image
        for i in range(len(contours)):
            r = randint(0, 255)
            g = randint(0, 255)
            b = randint(0, 255)
            cv2.fillConvexPoly(image, contours[i], (r, g, b))
        cv2.drawContours(image, contours, -1, (0, 255, 255), 1)
        # request_picture = base64.b64decode(request_picture)
        cv2.imwrite('heatmap.jpg',heatmap)
        cv2.imwrite('contours.jpg', image)
        path = os.path.join(sys.path[0], 'heatmap.jpg')
        result = {'path': path,'imageCount':siteid,'b64Image':image_to_base64(heatmap)}
        logging.info('process successfully, time : %f'%(time.time() - start_time))
        # logging.info('successfully compare face,similarity is {:f}'.format(degree_of_similarity))
        return json.dumps(result)
    except Exception as err:
        result = {'path': '', 'imageCount': 0, 'b64Image': ''}
        logging.error(traceback.format_exc())
        logging.info('failed to parse')
        return json.dumps(result)

@app.route('/')
def hello_world():
    return 'Hello World!'

if __name__ == '__main__':
    model_path = './py_server/weights-185-4-[0.640].pth'
    logging.getLogger().setLevel(logging.INFO)
    _particle = Particle(model_path)
    app.run(host='127.0.0.1', port=8087, debug=True)



