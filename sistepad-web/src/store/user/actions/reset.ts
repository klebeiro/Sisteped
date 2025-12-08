import { createAsyncThunk } from "@reduxjs/toolkit";

export const resetUserStateAction = createAsyncThunk(
  "user/reset",
    async (): Promise<void> => {}
);