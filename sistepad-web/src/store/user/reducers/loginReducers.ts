import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { UserState } from "../state";
import { loginAction } from "../actions/login";

export const loginReducers = (builder : ActionReducerMapBuilder<UserState>) =>{
    builder
    .addCase(loginAction.pending, (state) => {
        state.loading = true;
        state.error = null;
    })
    .addCase(loginAction.fulfilled, (state, action) => {
        state.loading = false;
        state.token = action.payload.Token;
        state.user = action.payload.User;
        state.error = null;
        state.successLogin = true;
    })
    .addCase(loginAction.rejected, (state, action) => {
        state.loading = false;
        state.successLogin = false;
        state.error = action.payload as string;
    });
}