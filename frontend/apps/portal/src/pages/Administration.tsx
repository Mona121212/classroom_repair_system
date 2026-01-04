import { Card, Alert, Space, Button } from 'antd';
import { useUser } from '../hooks/useUser';
import { useNavigate } from 'react-router-dom';

export default function Administration() {
  const { user, loading, isAdmin } = useUser();
  const navigate = useNavigate();

  if (loading) {
    return (
      <div style={{ padding: 24, textAlign: 'center' }}>
        <p>Loading...</p>
      </div>
    );
  }

  if (!isAdmin) {
    return (
      <div style={{ padding: 24 }}>
        <Alert
          message="Access Denied"
          description="You do not have permission to access the administration panel. Only administrators can access this page."
          type="error"
          showIcon
          action={
            <Button size="small" onClick={() => navigate('/')}>
              Go Home
            </Button>
          }
        />
      </div>
    );
  }

  return (
    <div style={{ padding: 24 }}>
      <h1>Administration Panel</h1>
      <p>Welcome, {user?.userName}. You have full administrative access.</p>

      <Space direction="vertical" style={{ width: '100%' }} size="large">
        <Card title="Facility Management" style={{ marginTop: 16 }}>
          <p>Manage all facilities in the system.</p>
          <ul>
            <li>Create new facilities</li>
            <li>Edit existing facilities</li>
            <li>Delete facilities</li>
            <li>View facility statistics</li>
          </ul>
          <Button type="primary" onClick={() => navigate('/facilities')}>
            Go to Facilities
          </Button>
        </Card>

        <Card title="Booking Management">
          <p>Manage all bookings across the system.</p>
          <ul>
            <li>View all bookings (not just your own)</li>
            <li>Approve or reject booking requests</li>
            <li>Cancel bookings</li>
            <li>View booking statistics</li>
          </ul>
          <Button type="primary" onClick={() => navigate('/bookings')}>
            Go to Bookings
          </Button>
        </Card>

        <Card title="User & Role Management">
          <p>Manage users and their roles.</p>
          <ul>
            <li>View all users</li>
            <li>Assign roles to users</li>
            <li>Manage permissions</li>
          </ul>
          <Alert
            message="Note"
            description="User management is available through the backend administration interface."
            type="info"
            style={{ marginTop: 16 }}
          />
        </Card>

        <Card title="System Settings">
          <p>Configure system-wide settings.</p>
          <ul>
            <li>Booking policies</li>
            <li>Facility rules</li>
            <li>Notification settings</li>
          </ul>
        </Card>
      </Space>
    </div>
  );
}

