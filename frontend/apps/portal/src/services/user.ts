import { apiClient } from './api';
import type { UserInfo } from '../types/user';

export const userService = {
  /**
   * Get current user information including roles
   * Uses ABP's application configuration endpoint which includes current user info
   */
  async getCurrentUser(): Promise<UserInfo | null> {
    try {
      // ABP Framework provides /api/abp/application-configuration endpoint
      // which includes current user information and roles
      const config = await apiClient.get<any>('/api/abp/application-configuration');
      
      const currentUser = config?.currentUser;
      if (!currentUser || !currentUser.isAuthenticated) {
        return null;
      }

      // Get roles from the current user object
      const roles = currentUser.roles || [];
      
      return {
        id: currentUser.id || '',
        userName: currentUser.userName || '',
        name: currentUser.name,
        email: currentUser.email,
        roles: roles,
      };
    } catch (error) {
      console.error('Failed to get user info:', error);
      return null;
    }
  },

  /**
   * Check if current user has a specific role
   */
  async hasRole(roleName: string): Promise<boolean> {
    const user = await this.getCurrentUser();
    return user?.roles.includes(roleName) || false;
  },

  /**
   * Check if current user is Admin
   */
  async isAdmin(): Promise<boolean> {
    return this.hasRole('Admin');
  },

  /**
   * Check if current user is Student
   */
  async isStudent(): Promise<boolean> {
    return this.hasRole('Student');
  },

  /**
   * Check if current user is Teacher
   */
  async isTeacher(): Promise<boolean> {
    return this.hasRole('Teacher');
  },
};

