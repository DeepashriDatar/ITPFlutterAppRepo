import 'package:intimepro_mobile/core/storage/drift/app_database.dart';

class SyncOperationRepository {
  SyncOperationRepository(this._database);

  final AppDatabase _database;

  Future<void> enqueue({
    required String id,
    required String entityType,
    required String entityId,
    required String operation,
    required String payloadJson,
    required String idempotencyKey,
  }) async {
    await _database.into(_database.syncOperations).insert(
          SyncOperationsCompanion.insert(
            id: id,
            entityType: entityType,
            entityId: entityId,
            operation: operation,
            payloadJson: payloadJson,
            idempotencyKey: idempotencyKey,
            nextAttemptAtUtc: DateTime.now().toUtc(),
          ),
        );
  }

  Future<List<SyncOperation>> pending() {
    return (_database.select(_database.syncOperations)
          ..where((SyncOperations tbl) => tbl.status.equals('Pending')))
        .get();
  }
}
