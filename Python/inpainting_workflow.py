import json
from urllib import request, parse
import random


def queue_prompt(prompt_workflow):
    p = {"prompt": prompt_workflow}
    data = json.dumps(p).encode('utf-8')
    req =  request.Request("http://127.0.0.1:8188/prompt", data=data)
    request.urlopen(req)    
    
# load the workflow from file, assign it to variable named prompt_workflow
prompt_workflow = json.load(open('AAEPlus_Inpainting_api.json', 'r', encoding='utf-8'))

load_userimg = prompt_workflow["110"]
load_mask = prompt_workflow["198"]
load_scribble = prompt_workflow["195"]

load_userimg["inputs"]["image"] = "C:\\Projekte\\ComfyUI_windows_portable\\ComfyUI\\output\\currentImg.png"
load_mask["inputs"]["image"] = "C:\\Projekte\\AIArtExtendedPlus\\Assets\\SavedTextures\\_CanvasTexture.png"
load_scribble["inputs"]["image"] = "C:\\Projekte\\AIArtExtendedPlus\\Assets\\SavedTextures\\_MaskTexture.png"

queue_prompt(prompt_workflow)