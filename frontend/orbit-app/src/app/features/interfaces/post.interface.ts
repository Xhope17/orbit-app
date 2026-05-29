export interface PostAuthor {
  profileId: string;
  username: string;
  displayName: string;
  avatarUrl: string | null;
}

export interface Post {
  id: string;
  author: PostAuthor;
  content: string;
  media: string[]; // Ajusta el tipo si tu backend devuelve objetos en lugar de strings de URLs
  likeCount: number;
  commentCount: number;
  isLiked: boolean;
  createdAt: string;
  updatedAt: string;
}
