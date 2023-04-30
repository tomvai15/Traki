import React from 'react';
import { Box, Tooltip } from '@mui/material';

type Props = {
  children?: React.ReactNode,
  disabled: boolean,
  title: string
};

export default function DisabledComponent ({children, disabled, title}: Props) {

  return (
    <Box>
      {disabled ? <Tooltip title={title}>
        <span>
          {children}
        </span>
      </Tooltip> :
        children}
    </Box>
  );
}
