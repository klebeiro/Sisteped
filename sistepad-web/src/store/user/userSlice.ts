import type { UserState } from "./state";
import { createSlice } from "@reduxjs/toolkit/react";
import { loginReducers, registerReducers, resetReducers } from "./reducers";

const initialUserState: UserState = {
  loading: false,
};

export const userSlice = createSlice({
  name: "user",
  initialState: initialUserState,
  reducers: {},
  extraReducers: (builder) => {
    loginReducers(builder);
    registerReducers(builder);
    resetReducers(builder);
    
    builder.addDefaultCase((state, action) => {});
  },
});
