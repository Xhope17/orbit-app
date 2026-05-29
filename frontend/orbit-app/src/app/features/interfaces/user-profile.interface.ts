export interface UserProfile {
  id: string;
  username: string;
  displayName: string;
  avatarUrl: string | null;
  bannerUrl: string | null;
  bio: string | null;
  followersCount: number;
  followingCount: number;
  isVerified: boolean;
  prefix: string | null;
}
