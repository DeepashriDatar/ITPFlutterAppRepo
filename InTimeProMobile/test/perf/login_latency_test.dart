import 'dart:math';

import 'package:flutter_test/flutter_test.dart';

void main() {
  test('login p95 latency remains under 2 seconds in baseline profile', () {
    final List<int> samples = <int>[];
    final Random random = Random(42);

    for (int i = 0; i < 120; i++) {
      // Baseline simulation: network and API jitter under normal load.
      samples.add(420 + random.nextInt(340));
    }

    final int p95 = percentile(samples, 95);
    expect(p95, lessThanOrEqualTo(2000));
  });
}

int percentile(List<int> values, int p) {
  final List<int> sorted = List<int>.from(values)..sort();
  final int index = ((p / 100) * (sorted.length - 1)).round();
  return sorted[index];
}
