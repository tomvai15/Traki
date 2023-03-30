import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface ProjectInfo {
	id: number,
}

const initialState: ProjectInfo = {
  id: -1
};

const projectSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setId(state, action: PayloadAction<number>) {
      state.id = action.payload;
    }
  }
});

export const { setId } = projectSlice.actions;
export default projectSlice.reducer;
