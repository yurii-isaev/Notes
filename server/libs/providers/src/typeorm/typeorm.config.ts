import { ConfigService } from '@nestjs/config';
import { config } from 'dotenv';
import { DataSource, DataSourceOptions } from 'typeorm';
import { join } from 'path';

config({
	path: join(process.cwd(), '.env'),
});

const configService = new ConfigService();

const options = (): DataSourceOptions => {
	const url = configService.get('DATABASE_URL');
	if (!url) throw new Error('Database url is empty');

	return {
		type: 'postgres',
		url,
		schema: 'public',
		logging: configService.get('NODE_ENV') === 'development',
		entities: [],
		migrations: [join(process.cwd(), 'migrations', '**', '*_migration.ts')],
		migrationsRun: true,
		metadataTableName: 'migrations',
	};
}

export const appDataSource = new DataSource(options());
