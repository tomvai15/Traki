import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Grid, Card, CardContent, Box, TextField, Button, ToggleButtonGroup, ToggleButton, } from '@mui/material';
import { Checklist, Section, UpdateSectionRequest } from 'contracts/protocol';
import { Item } from 'contracts/protocol/items';
import { sectionService } from 'services';
import { CreateChecklistCard } from 'features/protocols/components/CreateChecklistCard';
import { CreateTableCard } from 'features/protocols/components/CreateTableCard';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { Table } from 'contracts/protocol/section/Table';
import { initialSection, defaulTable, defaultChecklist } from 'features/protocols/data';
import { validate, validationRules } from 'utils/textValidation';

export function CreateSectionPage() {
  const navigate = useNavigate();
  const { protocolId } = useParams();

  const [section, setSection] = useState<Section>(initialSection);
  const [table, setTable] = useState<Table>(defaulTable);
  const [checklist, setChecklist] = useState<Checklist>(defaultChecklist);

  const [sectionType, setSectionType] = useState<string>('checklist');
  const [canCreate, setCanCreate] = useState<boolean>(true);

  function validInputs() {
    return (validSection() && 
            validChecklist() &&
            validTable());
  }

  function validSection() {
    return !validate(section?.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid;
  }

  function validChecklist() {
    return !checklist?.items.map(x => !validate(x.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid).some((value) => value == false);
  }

  function validTable() {
    return !table?.tableRows[0].rowColumns.map(x => !validate(x.value, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid).some((value) => value == false);
  }

  function updateItems (items: Item[]) {
    const newChecklist = {...checklist, items: items};
    setChecklist(newChecklist);
  }

  function setRow(row: TableRow) {
    setTable({...table, tableRows: [row]});
  }

  async function createSection() {
    const sectionTocreate: Section = {
      ...section, 
      checklist: sectionType == 'checklist' ? checklist : undefined,
      table: sectionType == 'table' ? table : undefined,
    };
    const updateSectionRequest: UpdateSectionRequest = {
      section: sectionTocreate
    };

    await sectionService.createSection(Number(protocolId), updateSectionRequest);
    setCanCreate(false);
  }

  function updateSectionName(newName: string) {
    setSection({...section, name: newName});
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Button onClick={() => navigate('/templates/protocols/' + protocolId)} variant='contained' >Go back</Button>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
              <TextField sx={{width: '50%'}}
                id="section-name"
                inputProps={{ maxLength: 100 }}
                error={validate(section?.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid}
                helperText={validate(section?.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).message}
                label="Section Name"
                variant="standard"
                value={section?.name}
                onChange={(e) => updateSectionName(e.target.value)}
              />
              <Button disabled={!canCreate || !validInputs()} onClick={() => createSection()} variant='contained'>Create</Button>
            </Box>
          </CardContent>    
          <CardContent sx={{paddingTop: 0}}>
            <ToggleButtonGroup
              value={sectionType}
              color="primary"
              exclusive
              aria-label="Platform"
              onChange={(e, value) => setSectionType(value)}
            >
              <ToggleButton value="checklist">Checklist</ToggleButton>
              <ToggleButton value="table">Table</ToggleButton>
            </ToggleButtonGroup>
            <Box sx={{marginTop: '10px', marginBottom: '10px'}}>    
              { sectionType == 'checklist' ? 
                <Box>
                  <CreateChecklistCard checklist={checklist} updateItems={updateItems}/>
                </Box> :
                <CreateTableCard row={table.tableRows[0]} setRow={setRow}/>}
            </Box>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}