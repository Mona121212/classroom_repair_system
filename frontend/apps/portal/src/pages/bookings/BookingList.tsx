import { useEffect, useState } from 'react';
import { Table, Card, Button, Tag, message } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { bookingsApi } from '../../services/bookings';
import type { BookingDto } from '../../types/booking';

export default function BookingList() {
  const navigate = useNavigate();
  const [bookings, setBookings] = useState<BookingDto[]>([]);
  const [loading, setLoading] = useState(false);

  const loadBookings = async () => {
    setLoading(true);
    try {
      const data = await bookingsApi.getList();
      setBookings(data);
    } catch (error: any) {
      message.error('Failed to load bookings: ' + (error.message || 'Unknown error'));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadBookings();
  }, []);

  const getStatusTag = (status: number) => {
    const statusMap: Record<number, { color: string; text: string }> = {
      0: { color: 'default', text: 'Draft' },
      1: { color: 'processing', text: 'Submitted' },
      2: { color: 'success', text: 'Approved' },
      3: { color: 'error', text: 'Rejected' },
      4: { color: 'warning', text: 'Cancelled' },
      5: { color: 'success', text: 'Completed' },
    };
    const statusInfo = statusMap[status] || { color: 'default', text: 'Unknown' };
    return <Tag color={statusInfo.color}>{statusInfo.text}</Tag>;
  };

  const columns = [
    {
      title: 'Booking No',
      dataIndex: 'bookingNo',
      key: 'bookingNo',
    },
    {
      title: 'Facility',
      dataIndex: 'facilityName',
      key: 'facilityName',
    },
    {
      title: 'Start Time',
      dataIndex: 'startTime',
      key: 'startTime',
      render: (time: string) => new Date(time).toLocaleString(),
    },
    {
      title: 'End Time',
      dataIndex: 'endTime',
      key: 'endTime',
      render: (time: string) => new Date(time).toLocaleString(),
    },
    {
      title: 'Participants',
      dataIndex: 'numberOfParticipants',
      key: 'numberOfParticipants',
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (status: number) => getStatusTag(status),
    },
    {
      title: 'Purpose',
      dataIndex: 'purpose',
      key: 'purpose',
      ellipsis: true,
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Card
        title="My Bookings"
        extra={
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => navigate('/bookings/create')}
          >
            New Booking
          </Button>
        }
      >
        <Table
          columns={columns}
          dataSource={bookings}
          loading={loading}
          rowKey="id"
          pagination={{ pageSize: 10 }}
        />
      </Card>
    </div>
  );
}
