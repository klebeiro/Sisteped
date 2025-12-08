import { configureStore } from "@reduxjs/toolkit";
import { useDispatch } from "react-redux";
import { userSlice } from "../user/userSlice";

export const storeState = configureStore({
  reducer: {
    user: userSlice.reducer,
  },
  devTools: true,
});

export type AppDispatch = typeof storeState.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>();
export type RootState = ReturnType<typeof storeState.getState>;