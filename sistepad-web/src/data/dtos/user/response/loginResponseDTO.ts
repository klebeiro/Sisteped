import type { BaseResponse } from "../../base/baseResponse";
import type { UserResponseDTO } from "./userResponseDTO";

export interface LoginResponseDTO extends BaseResponse {
  Token: string;
  User: UserResponseDTO;
}
