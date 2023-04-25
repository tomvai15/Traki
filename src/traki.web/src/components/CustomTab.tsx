import React from 'react';
import { Box } from '@mui/material';

type Props = {
  children?: React.ReactNode,
  value: number
  index: number
};

export default function CustomTab ({children, value, index}: Props) {
  return value == index ? <Box>{children}</Box> : <></>;
}
