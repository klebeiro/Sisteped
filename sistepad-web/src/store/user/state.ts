import type { UserResponseDTO } from "@/data/dtos/user/response/userResponseDTO";

export interface UserState{
    token?: string | null;
    user?: UserResponseDTO | null;
    loading: boolean;
    error?: string | null;
    successLogin?: boolean;
    successRegister?: boolean;
}