import React from 'react';
import { DefectStatus } from '../../../contracts/drawing/defect/DefectStatus';
import { StatusChange } from '../../../contracts/drawing/defect/StatusChange';
import { View } from 'react-native';
import { Button, Chip, Text, useTheme } from 'react-native-paper';


type StatusChangeItemProps = {
  statusChange: StatusChange
}

function defectStatusToText(defectStatus: DefectStatus): string {
  switch (defectStatus) {
    case DefectStatus.Fixed: return 'Fixed';
    case DefectStatus.NotFixed: return 'Not fixed';
    case DefectStatus.NotDefect: return 'Not a defect';
    case DefectStatus.Unfixable: return 'Unfixable';
  }
}

export function StatusChangeItem ({statusChange}: StatusChangeItemProps) {
  const { colors } = useTheme();
  return (
    <View style={{display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center', width: '100%'}}>
      <Chip style={{ backgroundColor: colors.secondary, borderRadius: 100}}>{defectStatusToText(statusChange.from)}</Chip>
      <Button textColor='black' style={{width: 1, margin: 0, padding: 0}} icon="arrow-right-thin">
      </Button>
      <Chip style={{backgroundColor: colors.primary, borderRadius: 100}}>{defectStatusToText(statusChange.to)}</Chip>
    </View>
  );
}