import  React, { useEffect, useState } from 'react';
import { View, Image, TouchableHighlight} from 'react-native';
import ImageView from "react-native-image-viewing";
import { List } from 'react-native-paper';

type ImageWithViewerProps = {
  width: number,
  height: number,
  source: string
}

export default function ImageWithViewer({ width, height, source }: ImageWithViewerProps) {
  const [viewerActive, setViewerActive] = useState<boolean>(false);

  return (
    <View>
      { source &&
        <ImageView
          images={[{uri: source}]}
          imageIndex={0}
          visible={viewerActive}
          onRequestClose={() => setViewerActive(false)}
        />}
      <TouchableHighlight   underlayColor="#DDDDDD" onPress={() => setViewerActive(true)}>
        <View>
          <Image source={{ uri: source}} style={{ width: width, height: height }} />
          <List.Icon color='grey' style={{position: 'absolute', bottom: 5, right: 5}}  icon={'magnify-plus-outline'}/>
        </View>
      </TouchableHighlight>
    </View>
  );
}
