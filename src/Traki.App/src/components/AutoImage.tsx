import React, { useEffect, useState } from 'react';
import { Image } from 'react-native';

type AutoImageProps = {
  width?: number,
  height?: number,
  source: string,
  sizeCallback?: (x: number, y: number) => void
}

type ImageSize = {
  width: number,
  height: number,
}

function AutoImage ({width, height, source, sizeCallback}: AutoImageProps) {
  const [imageSize, setImageSize] = useState<ImageSize>();

  /* eslint-disable */
  useEffect(() => {
    Image.getSize(source, (imageWidth, imageHeight) => {

      let newWidth = imageWidth;
      let newHeight = imageHeight;
      if (width == undefined && height != undefined) {
        newWidth = (imageWidth*height)/imageHeight;
        newHeight = height;
      } else if (width != undefined && height == undefined) {
        newWidth = width;
        newHeight = (imageHeight*width)/imageWidth;
      }
      
      if (sizeCallback) {
        sizeCallback(newWidth, newHeight);
      }
      setImageSize({width: newWidth, height: newHeight});}
    );
  }, [width, height, source]);
  /* eslint-disable */

  return (
    <Image style={{  width: imageSize?.width , height: imageSize?.height}} source={{ uri: source }} />
  );
}

export default AutoImage;