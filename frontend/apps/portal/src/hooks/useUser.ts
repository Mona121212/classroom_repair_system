import { useState, useEffect } from 'react';
import { userService } from '../services/user';
import type { UserInfo } from '../types/user';

export function useUser() {
  const [user, setUser] = useState<UserInfo | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadUser = async () => {
      try {
        const userInfo = await userService.getCurrentUser();
        setUser(userInfo);
      } catch (error) {
        console.error('Failed to load user:', error);
        setUser(null);
      } finally {
        setLoading(false);
      }
    };

    loadUser();
  }, []);

  const isAdmin = user?.roles.includes('Admin') || false;
  const isStudent = user?.roles.includes('Student') || false;
  const isTeacher = user?.roles.includes('Teacher') || false;

  return {
    user,
    loading,
    isAdmin,
    isStudent,
    isTeacher,
  };
}

