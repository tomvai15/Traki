import React, { useEffect, useState } from 'react';
import { Box, Link as BreadLink, Breadcrumbs, Button, Card, CardContent, Grid, TextField, Typography } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { Section } from '../../../contracts/protocol/Section';
import { UpdateSectionRequest } from '../../../contracts/protocol/UpdateSectionRequest';
import { Item } from '../../../contracts/protocol/items/Item';
import sectionService from '../../../services/section-service';
import { CreateChecklistCard } from 'features/protocols/components/CreateChecklistCard';
import { Table } from 'contracts/protocol/section/Table';
import { CreateTableCard } from 'features/protocols/components/CreateTableCard';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { RowColumn } from 'contracts/protocol/section/RowColumn';
import { validate, validationRules } from 'utils/textValidation';
import { ProtectedComponent } from 'components/ProtectedComponent';
import { useAlert } from 'hooks/useAlert';
import { useNotFoundCatcher } from 'hooks/useNotFoundCatcher';

const initialSection: Section = {
  id: 1,
  name: 'General check',
  priority: 1,
  checklist: undefined,
  table: undefined,
  protocolId: 0
};

export function EditSectionPage() {
  const {catchNotFound} = useNotFoundCatcher();
  const navigate = useNavigate();
  const { displaySuccess  } = useAlert();

  const { protocolId, sectionId } = useParams();
  const [section, setSection] = useState<Section>(initialSection);
  const [checklist, setChecklist] = useState<Checklist>();
  const [table, setTable] = useState<Table>();

  const [sectionType, setSectionType] = useState<string>('checklist');
  
  const [initialTableJson,  setInitialTableJson] = useState<string>('');
  const [initialChecklistJson,  setInitialChecklistJson] = useState<string>('');
  const [initialSectionJson,  setInitialSectionJson] = useState<string>('');

  useEffect(() => {
    catchNotFound(() => fetchSection());
  }, []);

  async function fetchSection() {
    const response = await sectionService.getSection(Number(protocolId), Number(sectionId));
    setSection(response.section);
    setInitialSectionJson(JSON.stringify(response.section));
    orderAndSetChecklist(response.section.checklist);
    orderAndSetTable(response.section.table);
  }

  function updateItems (items: Item[]) {
    if (!checklist) {
      return;
    }
    const newChecklist: Checklist = {...checklist, items: items};
    setChecklist(newChecklist);
  }

  function setRow(row: TableRow) {
    if (!table) {
      return;
    }
    setTable({...table, tableRows: [row]});
  }

  function canUpdate() {
    console.log(checklist == undefined ? true : validChecklist());
    return  (JSON.stringify(section) != initialSectionJson || 
            JSON.stringify(checklist) != initialChecklistJson ||
            JSON.stringify(table) != initialTableJson) && 
            (validSection() && 
            (checklist == undefined ? true : validChecklist()) && 
            (table == undefined ? true : validTable()));
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

  function orderAndSetTable(table?: Table) {
    if (!table) {
      setInitialTableJson(JSON.stringify(undefined));
      return;
    }

    const firstRow = table.tableRows[0];
    const columns: RowColumn[] = firstRow.rowColumns.map((column, index) => {
      return {...column, id: index};
    });
  
    const row: TableRow = {...firstRow, rowColumns: columns};
    setSectionType('table');
    const newTable: Table = {...table, tableRows: [row]};
    setTable(newTable);
    setInitialTableJson(JSON.stringify(newTable));
  }

  function orderAndSetChecklist(checklistToSort?: Checklist) {
    if (!checklistToSort) {
      setInitialChecklistJson(JSON.stringify(undefined));
      return;
    }
    setSectionType('checklist');
    const sortedItems = [...checklistToSort.items];
    sortedItems.sort((a, b) => a.priority - b.priority);

    const copiedChecklist: Checklist = {...checklistToSort, items: sortedItems};

    setChecklist(copiedChecklist);
    setInitialChecklistJson(JSON.stringify(copiedChecklist));
  }

  async function updateSection() {
    const sectionTocreate: Section = {
      ...section, 
      checklist: sectionType == 'checklist' ? checklist : undefined,
      table: sectionType == 'table' ? table : undefined,
    };

    const updateSectionRequest: UpdateSectionRequest = {
      section: sectionTocreate
    };

    await sectionService.updateSection(Number(protocolId), Number(sectionId), updateSectionRequest);
    if (sectionType == 'checklist') {
      setInitialChecklistJson(JSON.stringify(checklist));
    } else if (sectionType == 'table') {
      setInitialTableJson(JSON.stringify(table));
    }
    setInitialSectionJson(JSON.stringify(section));
    displaySuccess("Section was updated successfully");
  }

  async function deleteSection() {
    await sectionService.deleteSection(Number(protocolId), Number(sectionId));
    navigate('/templates/protocols');
  }

  function updateSectionName(newName: string) {
    setSection({...section, name: newName});
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <BreadLink color="inherit" href="/templates/protocols">
            Protocol Templates
          </BreadLink>
          <Typography color="text.primary">Section</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
              <TextField sx={{width: '50%'}}
                id="section-name"
                inputProps={{ maxLength: 100 }}
                error={validate(section?.name, [validationRules.noSpecialSymbols]).invalid}
                helperText={validate(section?.name, [validationRules.noSpecialSymbols]).message}
                label="Section Name"
                variant="standard"
                value={section?.name}
                onChange={(e) => updateSectionName(e.target.value)}
              />
              <Box>
                <ProtectedComponent role={"ProjectManager"}>
                  <Button id='update-section' sx={{marginRight: 1}} disabled={!canUpdate()} onClick={() => updateSection()} variant='contained'>Save</Button>
                  <Button id='delete-section' onClick={() => deleteSection()} color='error' variant='contained'>Delete</Button>
                </ProtectedComponent>
              </Box>
            </Box>
          </CardContent>    
          <CardContent sx={{ display: 'flex', flexDirection: 'column', paddingTop: 0}}>
            {table && <CreateTableCard row={table.tableRows[0]} setRow={setRow}/>}
            {checklist && <CreateChecklistCard checklist={checklist} updateItems={updateItems}/>}
          </CardContent>    
        </Card>
      </Grid>
    </Grid>
  );
}