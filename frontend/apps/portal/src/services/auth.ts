import { apiClient } from './api';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  access_token: string;
  token_type: string;
  expires_in: number;
}

export const authService = {
  /**
   * 登录并获取访问令牌
   */
  async login(username: string, password: string): Promise<string> {
    const formData = new URLSearchParams();
    formData.append('grant_type', 'password');
    formData.append('username', username);
    formData.append('password', password);
    formData.append('client_id', 'ServiceDesk_App');
    formData.append('scope', 'ServiceDesk');

    try {
      // Use relative path so Vite proxy can forward the request
      const response = await fetch('/connect/token', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: formData.toString(),
      });

      if (!response.ok) {
        const error = await response.text();
        throw new Error(error || 'Login failed');
      }

      const data: LoginResponse = await response.json();
      apiClient.setToken(data.access_token);
      return data.access_token;
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  },

  /**
   * 登出
   */
  logout(): void {
    apiClient.clearToken();
    window.location.href = '/login';
  },

  /**
   * 检查是否已登录
   */
  isAuthenticated(): boolean {
    return apiClient.getToken() !== null;
  },

  /**
   * 获取当前Token
   */
  getToken(): string | null {
    return apiClient.getToken();
  },
};

