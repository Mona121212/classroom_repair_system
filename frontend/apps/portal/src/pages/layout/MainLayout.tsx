import { Outlet, Link } from 'react-router-dom';
import { Layout, Menu, Button, Dropdown, Avatar } from 'antd';
import {
    HomeOutlined,
    BuildOutlined,
    CalendarOutlined,
    LogoutOutlined,
    UserOutlined,
    SettingOutlined,
} from '@ant-design/icons';
import { authService } from '../../services/auth';
import { useUser } from '../../hooks/useUser';

const { Header, Sider, Content } = Layout;

export default function MainLayout() {
  const { user, loading, isAdmin } = useUser();

  const handleLogout = () => {
    authService.logout();
  };

  const menuItems = [
    {
      key: '/',
      icon: <HomeOutlined />,
      label: <Link to="/">Home</Link>,
    },
    {
      key: '/facilities',
      icon: <BuildOutlined />,
      label: <Link to="/facilities">Facilities</Link>,
    },
    {
      key: '/bookings',
      icon: <CalendarOutlined />,
      label: <Link to="/bookings">My Bookings</Link>,
    },
    // Only show Administration menu for Admin users
    ...(isAdmin ? [{
      key: '/administration',
      icon: <SettingOutlined />,
      label: <Link to="/administration">Administration</Link>,
    }] : []),
  ];

  const userMenuItems = [
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Logout',
      onClick: handleLogout,
    },
  ];

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Header style={{ background: '#001529', padding: '0 24px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <div style={{ color: 'white', fontSize: 18, fontWeight: 'bold' }}>ServiceDesk</div>
        <Dropdown menu={{ items: userMenuItems }} placement="bottomRight">
          <Button type="text" style={{ color: 'white' }}>
            <Avatar icon={<UserOutlined />} style={{ marginRight: 8 }} />
            {loading ? 'Loading...' : (user?.userName || 'User')}
            {isAdmin && <span style={{ marginLeft: 8, fontSize: 12, opacity: 0.8 }}>(Admin)</span>}
          </Button>
        </Dropdown>
      </Header>
      <Layout>
        <Sider width={200} style={{ background: '#fff' }}>
          <Menu
            mode="inline"
            selectedKeys={[window.location.pathname]}
            items={menuItems}
            style={{ height: '100%', borderRight: 0 }}
          />
        </Sider>
        <Layout style={{ padding: '24px' }}>
          <Content style={{ background: '#fff', padding: 24, minHeight: 280 }}>
            <Outlet />
          </Content>
        </Layout>
      </Layout>
    </Layout>
  );
}

