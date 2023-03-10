import  React, { useEffect, useState } from 'react';
import { ScrollView, View } from 'react-native';
import { Avatar, Button, Card, List, Text } from 'react-native-paper';
import { image } from './test';


const LeftContent = props => <Text variant="titleLarge">10/14</Text>
const Wrench = props => <Avatar.Icon size={50} style={{backgroundColor:'red'}}  icon="wrench" />

export default function ProductScreen() {

  const [expanded, setExpanded] = React.useState(true);

  const handlePress = () => setExpanded(!expanded);

  useEffect(() => {
  },[]);

  return (
    <ScrollView>
      <Card>
        <Card.Content >
          <Text variant="titleLarge">Šiluminis mazgas</Text>
          <Text variant="bodyMedium">Busena: gaminamas</Text>
        </Card.Content>
        <Card.Cover style={{height:300}} source={{ uri: image }} />
        <Card.Actions>
          <Button>Redaguoti</Button>
          <Button>Keisti Būseną</Button>
        </Card.Actions>
      </Card>

      <Card style={{marginTop:10}}>
        <Card.Title title="Pridėti defektą" subtitle="Card Subtitle" left={Wrench} />
      </Card>

      <List.Section title="Inspekcijos">
        <Card style={{marginTop:10}}>
          <Card.Title title="Peržiūrėti inspekcijas" subtitle="Card Subtitle" right={LeftContent} />
        </Card>
        <Card style={{marginTop:10}}>
          <Card.Title title="Peržiūrėti inspekcijas" subtitle="Card Subtitle" right={LeftContent} />
        </Card>
      </List.Section>
    </ScrollView>
  );
}