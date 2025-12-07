import type { UserState } from "./state";
import { createSlice } from "@reduxjs/toolkit/react";
import { loginReducers, registerReducers } from "./reducers";

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

    builder.addDefaultCase((state, action) => {});
  },
});
