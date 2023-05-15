import React from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@mui/material';

type Props = {
  open: boolean,
  handleClose: () => void,
  title: string,
  body: string,
  action: () => void
};

export function DeleteItemDialog({open, handleClose, title, body, action }: Props) {
  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <DialogContentText>
          {body}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button color='inherit' onClick={handleClose}>Cancel</Button>
        <Button id='confirm' color='error' onClick={action}>Delete</Button>
      </DialogActions>
    </Dialog>
  );
}