import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { Form, Input, DatePicker, InputNumber, Button, Card, message, Space } from 'antd';
import { facilitiesApi } from '../../services/facilities';
import { bookingsApi } from '../../services/bookings';
import type { FacilityDto } from '../../types/facility';
import type { CreateBookingDto } from '../../types/booking';
import dayjs, { Dayjs } from 'dayjs';

const { TextArea } = Input;
const { RangePicker } = DatePicker;

export default function CreateBooking() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [form] = Form.useForm();
  const [facility, setFacility] = useState<FacilityDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const facilityId = searchParams.get('facilityId');

  useEffect(() => {
    if (facilityId) {
      loadFacility(facilityId);
      form.setFieldsValue({ facilityId });
    }
  }, [facilityId]);

  const loadFacility = async (id: string) => {
    setLoading(true);
    try {
      const data = await facilitiesApi.getById(id);
      setFacility(data);
      form.setFieldsValue({
        numberOfParticipants: 1,
        maxParticipants: data.capacity || undefined,
      });
    } catch (error: any) {
      message.error('Failed to load facility information: ' + (error.message || 'Unknown error'));
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (values: any) => {
    setSubmitting(true);
    try {
      const [startTime, endTime] = values.timeRange as [Dayjs, Dayjs];
      const bookingData: CreateBookingDto = {
        facilityId: values.facilityId,
        startTime: startTime.toISOString(),
        endTime: endTime.toISOString(),
        purpose: values.purpose,
        numberOfParticipants: values.numberOfParticipants,
      };

      const result = await bookingsApi.create(bookingData);
      message.success(`Booking created successfully! Booking No: ${result.bookingNo}`);
      navigate('/bookings');
    } catch (error: any) {
      const errorMessage = error.response?.data?.error?.message || error.message || 'Failed to create booking';
      message.error(errorMessage);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={{ padding: 24 }}>
      <Card title="Create Booking" loading={loading}>
        {facility && (
          <div style={{ marginBottom: 16, padding: 12, background: '#f5f5f5', borderRadius: 4 }}>
            <strong>Facility:</strong> {facility.name} | 
            <strong> Type:</strong> {facility.type || 'N/A'} | 
            <strong> Capacity:</strong> {facility.capacity ?? 'N/A'}
          </div>
        )}

        <Form
          form={form}
          layout="vertical"
          onFinish={handleSubmit}
          initialValues={{
            numberOfParticipants: 1,
          }}
        >
          <Form.Item
            name="facilityId"
            label="Facility ID"
            rules={[{ required: true, message: 'Please select a facility' }]}
            hidden
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="timeRange"
            label="Booking Time"
            rules={[{ required: true, message: 'Please select booking time' }]}
          >
            <RangePicker
              showTime
              format="YYYY-MM-DD HH:mm"
              style={{ width: '100%' }}
            />
          </Form.Item>

          <Form.Item
            name="numberOfParticipants"
            label="Number of Participants"
            rules={[
              { required: true, message: 'Please enter number of participants' },
              { type: 'number', min: 1, message: 'Number of participants must be at least 1' },
              facility?.capacity
                ? {
                    type: 'number',
                    max: facility.capacity,
                    message: `Number of participants cannot exceed facility capacity (${facility.capacity})`,
                  }
                : {},
            ]}
          >
            <InputNumber
              min={1}
              max={facility?.capacity}
              style={{ width: '100%' }}
              placeholder="Enter number of participants"
            />
          </Form.Item>

          <Form.Item
            name="purpose"
            label="Purpose"
            rules={[
              { required: true, message: 'Please enter booking purpose' },
              { max: 256, message: 'Purpose description cannot exceed 256 characters' },
            ]}
          >
            <TextArea
              rows={4}
              placeholder="Describe the purpose of this booking..."
            />
          </Form.Item>

          <Form.Item>
            <Space>
              <Button type="primary" htmlType="submit" loading={submitting}>
                Submit Booking
              </Button>
              <Button onClick={() => navigate('/facilities')}>
                Cancel
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}
