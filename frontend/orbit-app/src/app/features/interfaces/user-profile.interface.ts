export interface UserProfile {
  id: string;
  username: string;
  displayName: string;
  avatarUrl: string | null;
  bannerUrl: string | null;
  bio: string;
  followersCount: number;
  followingCount: number;
  isVerified: boolean;
  prefix: string | null;
  isPrivate: boolean; // falta implementar en el backend
  isFollowing?: boolean;
}

export interface UpdateProfileRequest {
  displayName: string;
  bio: string;
  isPrivate: boolean;
}


