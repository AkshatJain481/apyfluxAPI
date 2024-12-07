import sys
import json
from io import BytesIO
from PIL import Image
import base64
from transformers import DetrImageProcessor, DetrForObjectDetection
import torch

def detect_objects(image_data):
    # Decode Base64 image data
    image = Image.open(BytesIO(base64.b64decode(image_data)))

    # Load pre-trained model and processor
    processor = DetrImageProcessor.from_pretrained("facebook/detr-resnet-50", revision="no_timm")
    model = DetrForObjectDetection.from_pretrained("facebook/detr-resnet-50", revision="no_timm")

    # Preprocess and make predictions
    inputs = processor(images=image, return_tensors="pt")
    outputs = model(**inputs)

    # Post-process results
    target_sizes = torch.tensor([image.size[::-1]])
    results = processor.post_process_object_detection(outputs, target_sizes=target_sizes, threshold=0.9)[0]

    # Format results as JSON
    detections = []
    for score, label, box in zip(results["scores"], results["labels"], results["boxes"]):
        box = [round(i, 2) for i in box.tolist()]
        detections.append({
            "label": model.config.id2label[label.item()],
            "confidence": round(score.item(), 3),
            "box": box
        })

    return detections

if __name__ == "__main__":
    # Read image data from standard input
    image_data = sys.stdin.read()

    # Perform object detection
    try:
        results = detect_objects(image_data)
        print(json.dumps(results))  # Print results as JSON
    except Exception as e:
        print(json.dumps({"error": str(e)}), file=sys.stderr)
        sys.exit(1)
