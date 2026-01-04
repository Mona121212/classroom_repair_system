export interface BookingDto {
  id: string;
  bookingNo: string;
  facilityId: string;
  facilityName: string;
  startTime: string;
  endTime: string;
  purpose: string;
  numberOfParticipants: number;
  status: number;
  creationTime: string;
}

export interface CreateBookingDto {
  facilityId: string;
  startTime: string;
  endTime: string;
  purpose: string;
  numberOfParticipants: number;
}

