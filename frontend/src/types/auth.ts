export interface UserProfile {
  id: number;
  name: string;
  email: string;
  role?: 'Admin' | 'User' ; 
  avatarUrl?: string; 
  authProvider?: 'github' | 'google'; 
}

export interface AuthResponse {
  token: string;
  user: UserProfile;
}
