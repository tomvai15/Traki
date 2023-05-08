import React from 'react';
import { TextInput } from 'react-native-paper';
import { CommentWithImage } from '../types/CommentWithImage';
import { View } from 'react-native';
import ImageWithViewer from '../../../components/ImageWithViewer';

type CommentProps = {
  defectComment: CommentWithImage
}

export function Comment ({defectComment}: CommentProps) {
  return (
    <View style={{display: 'flex', flexDirection: 'row', paddingBottom: 10, width: '100%'}}>
      <TextInput value={defectComment.defectComment.text} 
        style={{flex: 1, marginRight: 5}}
        mode='outlined'
        multiline={true}
        editable={false}
        theme={{ roundness: 10, colors: { outline: '#7979796C', background: '#B9B9B940'}}}>
      </TextInput>
      { defectComment.imageBase64 != '' && <ImageWithViewer source={defectComment.imageBase64} width={100} height={100}></ImageWithViewer>}
    </View>
  );
}
