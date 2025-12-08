import type { BaseResponse } from "@/data/dtos/base/baseResponse";
import type { LoginRequestDTO } from "@/data/dtos/user/request/loginRequestDTO";
import type { RegisterUserRequestDTO } from "@/data/dtos/user/request/registerUserRequestDTO";
import type { LoginResponseDTO } from "@/data/dtos/user/response/loginResponseDTO";
import { AxiosClient } from "@/utils/axiosClient";

export class UserService {
    async login(loginRequestDTO: LoginRequestDTO): Promise<LoginResponseDTO> {
    const response = await AxiosClient.post<LoginResponseDTO>(
      "/user/login",
      loginRequestDTO
    );

    if (response.status !== 200) throw new Error(response.data.message);

    return response.data;
  }

  async createUser(
    registerUserRequestDTO: RegisterUserRequestDTO
  ): Promise<void> {
    const response = await AxiosClient.post<BaseResponse>(
      "/user/create",
      registerUserRequestDTO
    );

    if (response.status !== 201) throw new Error(response.data.message);
  }
}
