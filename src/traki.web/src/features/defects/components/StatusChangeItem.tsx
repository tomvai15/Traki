import React from 'react';
import { Box, Chip } from '@mui/material';
import { StatusChange } from 'contracts/drawing/defect/StatusChange';
import { DefectStatus } from 'contracts/drawing/defect/DefectStatus';
import ArrowRightAltIcon from '@mui/icons-material/ArrowRightAlt';

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
  return (
    <Box sx={{display: 'flex', marginTop: '10px', gap: '10px', flexDirection: 'row', alignItems: 'center'}}>
      <Chip id='activity-status-field-from'  
        sx={{
          '& .MuiChip-label': {
            overflow: 'visible !important',
          },
        }} 
        label={defectStatusToText(statusChange.from)}></Chip> <ArrowRightAltIcon/> <Chip id='activity-status-field-to' color='primary' label={defectStatusToText(statusChange.to)}></Chip>
    </Box>
  );
}