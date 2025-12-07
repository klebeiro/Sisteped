import { createAsyncThunk } from "@reduxjs/toolkit";
import { UserService } from "../../../services/userService";
import type { RegisterUserRequestDTO } from "@/data/dtos/user/request/registerUserRequestDTO";

export const registerUserAction = createAsyncThunk(
  "user/register",
  async (
    registerUserRequestDTO: RegisterUserRequestDTO,
    thunkAPI
  ): Promise<void> => {
    try {
      await new UserService().createUser(registerUserRequestDTO);
    } catch (error: any) {
      thunkAPI.rejectWithValue(error.message);
    }
  }
);