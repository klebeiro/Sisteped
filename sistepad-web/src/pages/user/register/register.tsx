import { type ReactElement, useEffect } from "react";
import "./register.style.css";
import { useForm, type SubmitHandler } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import {
  useAppDispatch,
  type RootState,
} from "@/store/redux/reduxConfiguration";
import { FormField } from "@/components";
import { registerUserAction } from "@/store/user/actions/register";
import { emailRegex } from "@/utils/appRegex";
import { toast } from "react-toastify";

type RegisterForm = {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
};

export function Register(): ReactElement {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const userState = useSelector((state: RootState) => state.user);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterForm>();

  const onSubmit: SubmitHandler<RegisterForm> = (data: RegisterForm) => {
    dispatch(
      registerUserAction({
        ...data,
        passwordConfirmation: data.confirmPassword,
      })
    );
  };

  useEffect(() => {
    if (userState.successRegister) navigate("/");
    else if (userState.error) toast.error(userState.error);
  }, [userState]);

  return (
    <div className="login-page">
      <h1 className="app-title">Sisteped</h1>
      <div className="container">
        <div className="form-container">
          <p className="title">Faça seu cadastro</p>
          <form className="form" onSubmit={handleSubmit(onSubmit)}>
            <FormField
              required
              type="text"
              label="Nome"
              fieldName="name"
              placleholder="Digite seu nome"
              register={register}
              error={!!errors.name}
              errorMessage="O nome é obrigatório"
            />
            <FormField
              required
              type="text"
              label="E-mail"
              fieldName="email"
              placleholder="email@dominio.com"
              patther={emailRegex}
              register={register}
              error={!!errors.email}
              errorMessage="O e-mail é obrigatório"
            />
            <FormField
              required
              label="Senha"
              type="password"
              fieldName="password"
              placleholder="Digite a senha"
              register={register}
              error={!!errors.password}
              errorMessage="A senha é obrigatória"
            />
            <FormField
              required
              label="Confirmação da senha"
              type="password"
              fieldName="confirmPassword"
              placleholder="Digite a senha novamente"
              register={register}
              error={!!errors.confirmPassword}
              errorMessage="As senhas informadas não coincidem"
            />
            <div className="subimit-box">
              <input className="subimit-button" type="submit" />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
