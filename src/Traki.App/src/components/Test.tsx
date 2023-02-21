import React, { useState } from 'react';
import { Button, Text, View } from 'react-native';

export default function Test() {
  const [count, setCount] = useState<number>(0);

  return (
    <View>
      <Text>Open up App.tsx to start working on your app!</Text>
      <Text>{count}</Text>
      <Button title='e' onPress={()=>setCount(count+1)}/>
    </View>
  );
}
