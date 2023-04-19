import { Snackbar, Alert, SnackbarCloseReason } from "@mui/material";
import { useAlert } from "hooks/useAlert";
import React from "react";
import { useRecoilState } from "recoil";
import { alertState } from "state/alert-state";

export default function AlertBar () {
  const [alert, _] = useRecoilState(alertState);
  const { clearNotification } = useAlert();

  const handleClose = (_: unknown, reason?: SnackbarCloseReason) =>
    reason !== "clickaway" && clearNotification();

  return (
    <Snackbar
      anchorOrigin={{
        vertical: "bottom",
        horizontal: "right"
      }}
      open={alert.open}
      autoHideDuration={alert.timeout}
      onClose={handleClose}
    >
      <Alert
        variant="filled"
        onClose={handleClose}
        severity={alert.type}
      >
        {alert.message}
      </Alert>
    </Snackbar>
  );
}