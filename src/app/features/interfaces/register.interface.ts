import { UserProfile } from './user-profile.interface';

export interface LoginData {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  profile: UserProfile;
}

export interface RegisterRequest {
  email: string;
  username: string;
  displayName: string;
  password: string;
  bio?: string;
}
