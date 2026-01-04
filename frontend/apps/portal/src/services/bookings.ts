import { apiClient } from './api';
import type { BookingDto, CreateBookingDto } from '../types/booking';

export const bookingsApi = {
  create: async (input: CreateBookingDto): Promise<BookingDto> => {
    return apiClient.post<BookingDto>('/api/app/booking', input);
  },

  getList: async (): Promise<BookingDto[]> => {
    return apiClient.get<BookingDto[]>('/api/app/booking');
  },

  getById: async (id: string): Promise<BookingDto> => {
    return apiClient.get<BookingDto>(`/api/app/booking/${id}`);
  },

  update: async (id: string, input: Partial<CreateBookingDto>): Promise<BookingDto> => {
    return apiClient.put<BookingDto>(`/api/app/booking/${id}`, input);
  },

  delete: async (id: string): Promise<void> => {
    return apiClient.delete<void>(`/api/app/booking/${id}`);
  },
};

