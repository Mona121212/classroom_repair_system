import { useEffect, useState } from 'react';
import { Table, Card, Input, Select, Button, Space, message } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { facilitiesApi } from '../../services/facilities';
import type { FacilityDto, FacilityGetListInput } from '../../types/facility';
import type { PagedResultDto } from '../../types/common';

const { Search } = Input;
const { Option } = Select;

export default function FacilityList() {
  const [facilities, setFacilities] = useState<FacilityDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [filters, setFilters] = useState<FacilityGetListInput>({
    skipCount: 0,
    maxResultCount: 10,
  });

  const loadFacilities = async () => {
    setLoading(true);
    try {
      const result: PagedResultDto<FacilityDto> = await facilitiesApi.getList(filters);
      setFacilities(result.items);
      setTotal(result.totalCount);
    } catch (error: any) {
      message.error('Failed to load facilities: ' + (error.message || 'Unknown error'));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadFacilities();
  }, [filters]);

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Type',
      dataIndex: 'type',
      key: 'type',
    },
    {
      title: 'Owner Unit',
      dataIndex: 'ownerUnit',
      key: 'ownerUnit',
    },
    {
      title: 'Capacity',
      dataIndex: 'capacity',
      key: 'capacity',
      render: (capacity: number | null) => capacity ?? 'N/A',
    },
    {
      title: 'Requires Approval',
      dataIndex: 'requiresApproval',
      key: 'requiresApproval',
      render: (requires: boolean) => (requires ? 'Yes' : 'No'),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_: any, record: FacilityDto) => (
        <Button type="link" onClick={() => window.location.href = `/facilities/${record.id}`}>
          View Details
        </Button>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Card>
        <Space direction="vertical" style={{ width: '100%' }} size="large">
          <Space>
            <Search
              placeholder="Search by owner unit"
              allowClear
              style={{ width: 200 }}
              onSearch={(value) => setFilters({ ...filters, campus: value || undefined, skipCount: 0 })}
            />
            <Select
              placeholder="Select type"
              allowClear
              style={{ width: 150 }}
              onChange={(value) => setFilters({ ...filters, type: value || undefined, skipCount: 0 })}
            >
              <Option value="Lab">Lab</Option>
              <Option value="Auditorium">Auditorium</Option>
              <Option value="Sports">Sports</Option>
              <Option value="Study Room">Study Room</Option>
            </Select>
            <Button icon={<SearchOutlined />} onClick={loadFacilities}>
              Search
            </Button>
          </Space>

          <Table
            columns={columns}
            dataSource={facilities}
            loading={loading}
            rowKey="id"
            pagination={{
              total,
              pageSize: filters.maxResultCount,
              current: (filters.skipCount || 0) / (filters.maxResultCount || 10) + 1,
              onChange: (page, pageSize) => {
                setFilters({
                  ...filters,
                  skipCount: (page - 1) * pageSize,
                  maxResultCount: pageSize,
                });
              },
            }}
          />
        </Space>
      </Card>
    </div>
  );
}
