import numpy as np
import cv2
import os,glob,sys
import torch
import logging
from torch.autograd import Variable
from imJNetV3 import Mobile_Unet

class Particle(object):
    def __init__(self, model_path):
        model = Mobile_Unet(num_classes=1, alpha=0.15, alpha_up=0.25)
        if model_path:
            model.cpu()
            model.eval()
            logging.info('resuming finetune from %s' % model_path)
            try:# map_location   将模型转换位置
                model.load_state_dict(torch.load(model_path, map_location=lambda storage, loc: storage))
            except KeyError:
                model = torch.nn.DataParallel(model)
                model.load_state_dict(torch.load(model_path))
        else:
            logging("Model path is not exist!!!")
        self.model = model

    def detect_patches(self, image, patch_size=(512, 512), factor=2):
        row, col, c = image.shape

        new_row = patch_size[0]
        new_col = patch_size[1]

        top = (new_row - row) // 2
        bottom = (new_row - row) // 2
        left = (new_col - col) // 2
        right = (new_col - col) // 2

        padding_image = cv2.copyMakeBorder(image, top, bottom, left, right, cv2.BORDER_CONSTANT)

        padding_image = cv2.resize(padding_image, (patch_size[1] * factor, patch_size[0] * factor))
        pred = self.heatmap(padding_image, False)
        heatmap = (pred * 255).astype(np.uint8)
        heatmap = cv2.resize(heatmap, (patch_size[1], patch_size[0]))
        return heatmap[top:row + top, left:col + left]

    def detect_whole(self, image, pitch_size):
        row, col, c = image.shape
        pitch_row_num = row // pitch_size[0]
        pitch_col_num = col // pitch_size[1]
        heatmap = image.copy()[:, :, 0] * 0

        for i in range(pitch_row_num):
            for j in range(pitch_col_num):
                y1 = i * pitch_size[0]
                y2 = (i + 1) * pitch_size[0]
                x1 = j * pitch_size[1]
                x2 = (j + 1) * pitch_size[1]
                # print(y1,y2,x1,x2)
                patch_image = image[y1: y2, x1: x2, :]
                #
                heatmap[y1: y2, x1: x2] = self.detect_patches(patch_image)
                # cv2.imshow("pair_image", pitch_image)
                # cv2.waitKey(0)
        return heatmap

    def heatmap(self, patch, Debug = True):
        # cv2.imwrite("live.jpg", patch)
        patch = cv2.cvtColor(patch, cv2.COLOR_BGR2RGB)
        patch = patch / 255.
        patch = torch.from_numpy(patch).permute(2, 0, 1).float()
        patch = patch[np.newaxis, :]
        patch = Variable(patch).cpu()
        outputs = self.model(patch)
        outputs = outputs.view(-1, outputs.size()[2], outputs.size()[3])
        outputs = outputs.cpu().data.numpy()
        patch = patch.cpu().data.numpy()
        # if score > self.threshold:
        #     pass
        if Debug:
            # print("liveness score:",score)
            # print(outputs[0])
            cv2.imshow("pred", outputs[0])
            face = cv2.cvtColor(np.transpose(patch[0], (1, 2, 0)), cv2.COLOR_RGB2BGR)
            x = (outputs[0] * 250)
            np.savetxt('feature.txt',outputs[0])
            cv2.imwrite("heatmap.jpg", x.astype(np.int))
            cv2.imshow("images", face)
            # cv2.waitKey(0)
        return outputs[0]
