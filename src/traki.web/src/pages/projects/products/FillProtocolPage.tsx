import React, { useEffect, useState } from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, Stack, Typography } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { Section } from '../../../contracts/protocol/Section';
import protocolService from '../../../services/protocol-service';
import sectionService from '../../../services/section-service';
import { FillSection } from 'features/protocols/components';
import { validate, validationRules } from 'utils/textValidation';
import { UpdateProtocolRequest } from 'contracts/protocol/UpdateProtocolRequest';

const initialProtocol: Protocol = {
  id: 1,
  name: '',
  sections: [],
  isTemplate: false,
  modifiedDate: '',
  isSigned: false,
  isCompleted: false
};

export function FillProtocolPage() {
  const navigate = useNavigate();
  const { projectId, productId, protocolId } = useParams();
  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  const [open, setOpen] = useState(false);

  function handleClose() {
    setOpen(false);
  }

  async function completeProtocol() {
    const request: UpdateProtocolRequest = {
      protocol: {...protocol, isCompleted: true},
      sections: sections
    };
    await protocolService.updateProtocol(Number(protocolId), request);
    return;
  }

  useEffect(() => {
    fetchProtocol();
  }, []);

  async function fetchProtocol() {
    const getProtocolsResponse = await protocolService.getProtocol(Number(protocolId));
    const getSectionsResponse = await sectionService.getSections(Number(protocolId));
    setProtocol(getProtocolsResponse.protocol);
    orderAndSetSections(getSectionsResponse.sections);
  }

  function orderAndSetSections(sectionsToSort: Section[]) {
    const sortedItems = [...sectionsToSort];

    console.log(sectionsToSort);
    sortedItems.sort((a, b) => a.priority - b.priority);
    setSections(sortedItems);
  }

  function validateNotEmpty(): boolean {
    return !sections.map(x=> isValidSection(x)).some(x=> x == false);
  }

  function isValidSection(section: Section): boolean {
    return !section.checklist?.items.map(x => { 
      return (
        (x.textInput == undefined ? true : !validate(x.textInput.value, [ validationRules.noSpecialSymbols]).invalid) &&
        (x.textInput == undefined ? true : !validate(x.textInput.value, [validationRules.noSpecialSymbols]).invalid) &&
        (x.question == undefined ? true : !validate(x.question.comment, [validationRules.noSpecialSymbols]).invalid)
      );
    }).some((value) => value == false);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Button onClick={() => navigate(`/projects/${projectId}/products/${productId}`)} variant='contained' >Go back</Button>
      </Grid>
      <Grid item xs={12} md={12} >
        <Stack direction={'row'} justifyContent={'space-between'} alignItems={'center'}>
          <Typography variant='h5'>{protocol.name}</Typography>
          {!protocol.isCompleted && <Button onClick={() => setOpen(true)} variant='contained'>Complete</Button>}
        </Stack>
      </Grid>
      {sections.map((section, index) => 
        <Grid key={index} item xs={12} md={12} >
          <FillSection protocolId={Number(protocolId)} completed={protocol.isCompleted} sectionId={section.id}></FillSection>
        </Grid>)}

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Complete protocol</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure want to complete protocol, no changes can be made after?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button color='inherit' onClick={handleClose}>Cancel</Button>
          <Button variant='contained' color='primary' onClick={completeProtocol}>Complte</Button>
        </DialogActions>
      </Dialog>
    </Grid>
  );
}