import { ApiResponse } from '../../shared/interfaces/apiResponse.interface';
import { UserProfile } from './user-profile.interface';

export interface LoginRequest {
  EmailOrUsername: string;
  password: string;
}

export interface LoginData {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  profile: UserProfile;
}

// export interface LoginResponse {
//     refreshToken(accessToken: any, refreshToken: any): unknown;
//     accessToken: any;
//     token: string;
// }

export type LoginResponse = ApiResponse<LoginData>;

export interface RegisterRequest {
  email: string;
  username: string;
  displayName: string;
  password: string;
  bio?: string;
}
export interface JwtPayload {
  sub: string;
  profile_id: string;
  unique_name: string;
  jti: string; // Identificador del token
  exp: number; // Fecha de expiración sirve para el auto-logout
  iss: string; // Issuer (OrbitApi)
  aud: string; // Audience (OrbitClient)
}
