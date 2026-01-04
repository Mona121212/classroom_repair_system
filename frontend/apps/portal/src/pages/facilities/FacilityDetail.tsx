import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Card, Descriptions, Button, message, Spin } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { facilitiesApi } from '../../services/facilities';
import type { FacilityDto } from '../../types/facility';

export default function FacilityDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [facility, setFacility] = useState<FacilityDto | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (id) {
      loadFacility(id);
    }
  }, [id]);

  const loadFacility = async (facilityId: string) => {
    setLoading(true);
    try {
      const data = await facilitiesApi.getById(facilityId);
      setFacility(data);
    } catch (error: any) {
      message.error('Failed to load facility details: ' + (error.message || 'Unknown error'));
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div style={{ padding: 24, textAlign: 'center' }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!facility) {
    return (
      <div style={{ padding: 24 }}>
        <Card>
          <p>Facility not found</p>
          <Button onClick={() => navigate('/facilities')}>Back to List</Button>
        </Card>
      </div>
    );
  }

  return (
    <div style={{ padding: 24 }}>
      <Card>
        <Button
          icon={<ArrowLeftOutlined />}
          onClick={() => navigate('/facilities')}
          style={{ marginBottom: 16 }}
        >
          Back to List
        </Button>

        <Descriptions title={facility.name} bordered column={2}>
          <Descriptions.Item label="Name">{facility.name}</Descriptions.Item>
          <Descriptions.Item label="Type">{facility.type || 'N/A'}</Descriptions.Item>
          <Descriptions.Item label="Owner Unit">{facility.ownerUnit || 'N/A'}</Descriptions.Item>
          <Descriptions.Item label="Capacity">{facility.capacity ?? 'N/A'}</Descriptions.Item>
          <Descriptions.Item label="Requires Approval">{facility.requiresApproval ? 'Yes' : 'No'}</Descriptions.Item>
          <Descriptions.Item label="Created At">
            {new Date(facility.creationTime).toLocaleString()}
          </Descriptions.Item>
          <Descriptions.Item label="Description" span={2}>
            {facility.description || 'No description'}
          </Descriptions.Item>
        </Descriptions>

        <div style={{ marginTop: 16 }}>
          <Button
            type="primary"
            onClick={() => navigate(`/bookings/create?facilityId=${facility.id}`)}
          >
            Book This Facility
          </Button>
        </div>
      </Card>
    </div>
  );
}
