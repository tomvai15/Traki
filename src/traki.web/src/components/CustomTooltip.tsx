import React from 'react';
import { Box, Tooltip } from '@mui/material';
import { makeStyles } from '@mui/styles';

const useStyles = makeStyles({
  arrow: {
    color: 'grey',
  },
});

type Props = {
  children?: React.ReactNode,
  title: string
};

export default function CustomTooltip ({children, title}: Props) {

  const classes = useStyles();

  return (
    <Tooltip
      sx={{fontSize: 40}}
      open={true}
      title={title}
      placement="top"
      arrow
      classes={{ arrow: classes.arrow }}

    >
      <Box>
        {children}
      </Box>
    </Tooltip>
  );
}
